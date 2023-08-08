import subprocess
import os

# Caminho para os projetos
caminho_backend = os.path.abspath("backend/HorasExtras")
caminho_frontend = os.path.abspath("frontend/HorasExtras")

# Comandos para executar os projetos
comando_backend = 'dotnet run'  # Comando de execução do projeto C#
comando_frontend = 'npm start'  # Comando de execução do projeto Angular

# Executar o projeto backend
subprocess.Popen(comando_backend, cwd=caminho_backend, shell=True)

# Executar o projeto frontend
subprocess.Popen(comando_frontend, cwd=caminho_frontend, shell=True)
