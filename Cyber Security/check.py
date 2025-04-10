from scapy.all import ARP, Ether, srp
import sys
import requests

interface = "Беспроводная сеть"

target_ip = "10.30.0.0/20"

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
    
def scan_network(ip):
    try:
        arp = ARP(pdst=ip)
        ether = Ether(dst="ff:ff:ff:ff:ff:ff")
        
        # packetni birlashtirish
        packet=ether/arp

        result = srp(packet, timeout=3, iface=interface, verbose=False)[0]

        devices=[]

        for sent, received in result:
            vendor = get_vendor(received.hwsrc)
            devices.append({'ip':received.psrc, 'mac': received.hwsrc, 'vendor': vendor})
        return devices
    except Exception as err:
        print(err)
        return []
    
def save_to_file(devices):
    with open("mac_addresses.txt", "w") as file:
        file.write("IP Address\t\tMAC Address\t\tVendor\n")
        file.writa("-" * 60 + "\n")

        for device in devices:
            file.write(f"{device['ip']}\t\t{device['mac']}\t\t{device['vendor']}\n")

def main():
    print("Network scanning started...")
    devices = scan_network(target_ip)
    if devices:
        print(f"Finded devices number: {len(devices)}")
        print("\nIP Address\t\tMAC Address\t\tVendor")
        print("-" * 60)

        for device in devices:
            print(f"{device['ip']}\t\t{device['mac']}\t\t{device['vendor']}")

        save_to_file(devices)
        print("Results saved via 'mac_addresses.txt'")
    else:
        print("Devices not finded")


if __name__ == "__main__":
    main()
