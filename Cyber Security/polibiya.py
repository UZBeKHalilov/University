# Polibiya kvadrati
alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"
size = 5

# Matritsa tuzish
def create_matrix():
    matrix = []
    for i in range(0, len(alphabet), size):
        matrix.append(list(alphabet[i:i + size]))
    return matrix

def encrypt(text):
    matrix = create_matrix()
    encrypte = ''
    for char in text:
        if char.isalpha():
            encrypted += encrypt_char(char, matrix) + ' '
    return encrypted.strip()
    
def decrypt_char(matrix, row, col):
    return matrix[int(row) - 1][int(col) - 1]

def decrypt(text):
    matrix = create_matrix()
    decrypted = ''
    pairs = code.split()
    for pair in pairs:
        if len(pair) == 2 and pair.isdigit():
           decrypted += decrypt_char(matrix, pair[0], pair[1])
    return decrypted

text = input("Matnni kiriting: ")
encrypted = encrypt(text)
decrypted = decrypt(encrypted)
print(f"Asl matn: {text}")
print(f"Shifrlangan matn: {encrypted}")
print(f"Deshifrlangan matn: {decrypted}")