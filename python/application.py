import tkinter as tk
import vlc

class YiyoPlayer:
    def __init__(self, window):
        self.window = window
        self.window.bind("f", self.toggle_fullscreen)
        self.window.bind("<Escape>", self.toggle_fullscreen)
        self.instance = vlc.Instance("--no-xlib")
        self.player = self.instance.media_player_new()
        self.player.set_hwnd(self.get_win_id())
        
    def get_win_id(self):
        self.window.update()
        return self.window.winfo_id()
        
    def play(self, url):

        try:
            media = self.instance.media_new(url)
            self.player.set_media(media)
            self.player.play()
        except:
            print("Cerrado")
        print("askjdf")
        
    def stop(self):
        self.player.stop()
    
    def toggle_fullscreen(self, event):
        if event.char == "f":
            # self.window.state = True
            self.window.attributes("-fullscreen", not self.window.attributes("-fullscreen"))
        else:
            self.window.attributes("-fullscreen", False)


ventana = tk.Tk()
ventana.geometry("800x450")
ventana.title("YiyoPlayer - 1.0.0.0")
reproductor = YiyoPlayer(ventana)




def leftKey(event):
    reproductor.play('')

ventana.bind('<Left>', leftKey)

#ventana.iconbitmap("icon.ico")


reproductor.play('')



ventana.mainloop()