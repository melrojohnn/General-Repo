#!/usr/bin/env python3
"""Hello World Multi Linguas.

Dependendo da lingua configurada no ambiente o programa exibe a mensagem 
correspondente.

Como usar:

Tenha a variável LANG devidamente configurada ex:

    export LANG=pt_BR

Execução:

    python3 hello.py
    ou
    ./hello.py
"""
__version__ = "0.1.2"
__author__ = "Johnnes Melro"
__license__ = "Unlicense"

import os

current_language = os.getenv("LANG", "en_US")[:5]

msg = "Hello, World!"


# Aqui aplicamos a condicional

if current_language == "pt_BR":
	msg = "Olá, Mundo!"
elif current_language == "it_IT":
	msg = "Ciao, Mondo!"
elif current_language == "fr_FR":
	msg = "Bonjour, Monde!"
elif current_language == "es_ES":
    msg = "Hola, Mundo!"
elif current_language == "en_US":
    msg = "Hello, World!"
elif current_language == "de_DE": 
    msg = "Hallo, Welt!"
print(msg)

