import socket
import threading
from cryptography.fernet import Fernet

#shifrlash kalitini generatsiya qilish
key = Fernet.generate_key()
cipher = Fernet(key)

def handle_client(client_socket, address):
    print(f"Connection from {address} has been established.")
    client_socket.send(key)  # Kalitni mijozga yuborish
    while True:
        try:
            # Mijozdan ma'lumotlarni qabul qilish
            encrypted_data = client_socket.recv(1024)
            if not encrypted_data:
                break

            # Ma'lumotlarni deshifrlash
            data = cipher.decrypt(encrypted_data).decode()
            print(f"Received({address}): {data}")

            # javob yuborish
            response = f"Server received: {data}"
            encrypted_response = cipher.encrypt(response.encode())
            client_socket.send(encrypted_response)
        except Exception as e:
            print(f"Error: {e}")
            break
    client_socket.close()
    print(f"Connection from {address} has been closed.")

def start_server():
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind(("127.0.0.1", 5555))
    server.listen(5)
    print("Server is listening on port 5555...")

    while True:
        client_socket, address = server.accept()
        print(f"Connection from {address} has been accepted.")
        client_thread = threading.Thread(target=handle_client, args=(client_socket, address))
        client_thread.start()

if __name__ == "__main__":
    start_server()