import scapy.all as scapy
from scapy.layers import http

def sniffer(interface):
    scapy.sniff(iface=interface, store=False, prn=proc_sniffed_pack)

def proc_sniffed_pack(pack):
    if pack.haslayer(http.HTTPRequest):
        print(pack.show())
        url = pack[http.HTTPRequest].Host + pack[http.HTTPRequest].Path
        print(url)
        if pack.haslayer(scapy.Raw):
            print(pack[scapy.Raw].load)

sniffer("Беспроводная сеть") 