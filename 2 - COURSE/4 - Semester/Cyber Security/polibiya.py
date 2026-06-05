# poliby kvadrati
alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ" # i VA j bir xil xisoblanadi
size = 5
# Matritsa tuzish
def create_matrix():
    matrix = []
    for i in range(0, len(alphabet), size):
        matrix.append(list(alphabet[i:i+size]))
    return matrix

def encrypt_char (matrix, char):
    char = "I" if char == 'J' else char.upper()
    for i in range (size):
        for j in range(size):
            if matrix [i][j] == char:
                return str(i+1) + str(j+1)
    return ''
# Matndan shofrlangan kod olish
def encrypt(text):
    matrix = create_matrix()
    encrypted = ''
    for char in text:
        if char.isalpha():
            encrypted += encrypt_char(matrix, char)
    return encrypted.strip()

# Kordinatani harfga o'tkazish
def decrypt_char(matrix, row, col):
    return matrix [int(row) -1] [int(col)-1]
# Shifrdan matnni tiklash
def decrypt(code):
    matrix = create_matrix()
    decrypted = ''
    if len(code) % 2 != 0 or not code.isdigit():
        return "Noto'g'ri shifrlangan kod formati!"
    for i in range(0, len(code), 2):
        row = code[i]
        col = code[i+1]
        if '1' <= row <= str(size) and '1' <= col <= str(size):
            decrypted += decrypt_char(matrix, row, col)
        else:
            return "Noto'g'ri shifrlangan kod qiymatlari!"
    return decrypted.strip()

# Misol
text = input("Matnni kiriting: ")
encrypted = encrypt(text)
decrypted = decrypt(encrypted)
print("Asl matn:", text)
print("Shifrlangan matn:", encrypted)
print("Shifrdan tiklangan matn:", decrypted)

encrypted_input = input("Shifrlangan matnni kiriting (deshifrlash uchun): ")
deshifrlangan_natija = decrypt(encrypted_input)
print("Shifrdan tiklangan matn:", deshifrlangan_natija)
