import tkinter as tk
import vlc

ventana = tk.Tk()
ventana.geometry("800x450")
ventana.title("YiyoPlayer - 1.0.0.0")

player = vlc.MediaPlayer()
media = None

def toggle_fullscreen(event):
    if event.char == "f":
        ventana.state = True
        ventana.attributes("-fullscreen", not ventana.attributes("-fullscreen"))
    else:
        ventana.attributes("-fullscreen", False)

def leftKey(event):
    print("\n\n\nasdnfasdf")
    try:
        player.stop()
        media = vlc.Media("https://www.w3schools.com/tags/movie.mp4")
        player.set_media(media)
    except:
        print("Cerrado")

ventana.bind("f", toggle_fullscreen)
ventana.bind("<Escape>", toggle_fullscreen)
ventana.bind('<Left>', leftKey)



media = vlc.Media("")
player.set_media(media)
player.video_set_mouse_input(False)

# Obtener el identificador de la ventana de tkinter y establecerlo como el controlador de video de vlc
player.set_hwnd(ventana.winfo_id())
player.play()
ventana.mainloop()


