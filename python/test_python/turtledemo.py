import turtle

def setup():
    window = turtle.Screen()
    window.title("Turtle Demo")
    window.exitonclick()  # Fecha a janela quando for clicada

def draw_square():
    leo = turtle.Turtle()
    leo.color("blue")
    for _ in range(4):
        leo.forward(100)
        leo.right(90)

def main():
    setup()
    draw_square()

if __name__ == "__main__":
    main()
