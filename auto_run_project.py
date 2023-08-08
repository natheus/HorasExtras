import subprocess
import os

# Caminho para os projetos
caminho_backend = os.path.abspath("backend/HorasExtras")
caminho_frontend = os.path.abspath("frontend/HorasExtras")

# Comandos para executar os projetos
comando_frontend = 'npm start'  # Comando de execução do projeto Frontend
comando_backend = "dotnet run"

# Comandos para instalar as dependências
comando_instalar_dependencias_back = "dotnet restore"
comando_instalar_dependencias_front = "npm install"

# Instalar as dependências do projeto backend
subprocess.Popen(comando_instalar_dependencias_back, cwd=caminho_backend, shell=True)

# Instalar as dependências do projeto frontend
subprocess.Popen(comando_instalar_dependencias_front, cwd=caminho_frontend, shell=True)

# Executar o projeto backend
subprocess.Popen(comando_backend, cwd=caminho_backend, shell=True)

# Executar o projeto frontend
subprocess.Popen(comando_frontend, cwd=caminho_frontend, shell=True)
