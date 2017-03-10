/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Client;

import java.io.FileInputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.net.Socket;
import java.nio.ByteBuffer;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import javafx.concurrent.Task;

/**
 *
 * @author Zaki
 */
public class SendWorker extends Task<Void>{

    private final Path file;
    private final String serverName;
    private final int portNumber;
    private final boolean compression;
    private final boolean encryption;
    
    public SendWorker(Path f, String sn, int pn, 
            boolean c, boolean e){
        this.file = f;
        this.serverName = sn;
        this.portNumber = pn;
        this.compression = c;
        this.encryption = e;
    }
    
    @Override
    protected Void call() throws Exception {
        int i = 0;
        byte[] buffer = new byte[512];
        updateProgress(i, Files.size(file));
        
        FileInputStream fis = null;
        Socket socket = null;
        OutputStream os = null;
        try {
            socket = new Socket(this.serverName, this.portNumber);
            os = socket.getOutputStream();
            fis = new FileInputStream(file.toFile());
            
            os.write(toBytes(this.getFileName().length()));
            os.write(toBytes(this.getFileName()));
            
            int dataSize;
            while ((dataSize = fis.read(buffer)) > -1) {
                if (isCancelled()) {
                    System.out.print("Sending cancelled" + System.lineSeparator());
                    updateMessage("Cancelled");
                    return null;
                }
                updateMessage("Sending...");
                os.write(buffer);
                i += dataSize;
                updateProgress(i, Files.size(file));
            }
            
        } finally {
            try {
                if(socket != null) socket.close();
                if(os != null) os.close();
                if(fis != null) fis.close();
            } catch (IOException ex) {
                System.out.print("Exception in 'SendWorker' : " + ex + System.lineSeparator());
            }
        }
        
        updateMessage("Done!");
        
        return null;
    }
    
    public static byte[] toBytes(String name){
        return name.getBytes(StandardCharsets.UTF_8);
    }
    
    public static byte[] toBytes(int value){
        return ByteBuffer.allocate(4).putInt(value).array();
    }
    
    public String getFileName(){
        return this.file.getFileName().toString();
    }
    
}
