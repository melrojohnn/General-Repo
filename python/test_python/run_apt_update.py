#!/usr/bin/env python3
"""
Script to execute 'sudo apt update' every hour.

"""

__version__ = "0.0.1"
__author__ = "Johnnes Melro"
__license__ = "Unlicense"

import subprocess
import time

def run_apt_update():
    # Execute the 'sudo apt update' command
    try:
        subprocess.run(['sudo', 'apt', 'update'], check=True)
        print("Command 'sudo apt update' executed successfully.")
    except subprocess.CalledProcessError as e:
        print(f"Error executing the command: {e}")

if __name__ == "__main__":
    # Run the 'sudo apt update' command
    run_apt_update()
    print("The script executed successfully and will now exit.")