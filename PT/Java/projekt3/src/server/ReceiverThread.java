package server;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.net.Socket;
import java.nio.ByteBuffer;
import java.nio.charset.StandardCharsets;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.zip.ZipEntry;
import java.util.zip.ZipInputStream;

/**
 *
 * @author zaki
 */
public class ReceiverThread implements Runnable {

    private Socket socket;
    private static final Logger log = Logger.getLogger(ReceiverThread.class.getName());
    private static int fileNumber = 0;
    private String fileName;
    
    public ReceiverThread(Socket socket) {
        this.socket = socket;
    }

    @Override
    public void run() {
        InputStream is = null;
        FileOutputStream fos = null;
        InputStreamReader isw = null;
        try {
            is = socket.getInputStream();
            isw =new InputStreamReader(is, "UTF-8");
            
            this.fileName = readName(is);
            fos = new FileOutputStream("F:\\New folder\\"+this.fileName);
           
            byte[] buffer = new byte[1024];
            int dataSize;
            while ((dataSize = is.read(buffer)) > -1) {
                fos.write(buffer, 0, dataSize);
            }
        } catch (IOException ex) {
            log.log(Level.WARNING, ex.getMessage(), ex);
        } finally {
            try {
                if(socket != null) socket.close();
                if(fos != null) fos.close();
                if(is != null) is.close();
            } catch (IOException ex) {
                log.log(Level.WARNING, ex.getMessage(), ex);
            }
        }
        ReceiverThread.fileNumber++;
        System.out.print("Received file "+ReceiverThread.fileNumber+" - "+
                this.fileName+System.lineSeparator());
    }
    
    public static int toInt(byte[] arr){
        ByteBuffer wrapped = ByteBuffer.wrap(arr); 
        return wrapped.getInt();
    }
    
    public static String readName(InputStream is) {
        try {
            byte[] tmpArr = new byte[4];
            is.read(tmpArr, 0, 4);
            int nameLen = toInt(tmpArr);
            tmpArr = new byte[nameLen];
            is.read(tmpArr, 0, nameLen);
            return new String(tmpArr, StandardCharsets.UTF_8);
        } catch (IOException ex) {
            Logger.getLogger(ReceiverThread.class.getName()).log(Level.SEVERE, null, ex);
        }
        return null;
    }
    
    /*
    public void run() {
        InputStream is = null;
        FileOutputStream fos = null;
        ZipInputStream zis = null;
        try {
            int buffer = 1024;
            byte[] data = new byte[buffer];
            
            is = socket.getInputStream();
            zis = new ZipInputStream(new BufferedInputStream(is));
            ZipEntry entry;
            
            while((entry = zis.getNextEntry()) != null) {
                fos = new FileOutputStream("F:\\New folder\\"+entry.getName());
                BufferedOutputStream dest = new BufferedOutputStream(fos, buffer);
                int count;
                while ((count = zis.read(data, 0, buffer)) != -1) {
                    dest.write(data, 0, count);
                }
            }
        } catch (IOException ex) {
            log.log(Level.WARNING, ex.getMessage(), ex);
        } finally {
            try {
                if(socket != null) socket.close();
                if(fos != null) fos.close();
                if(is != null) is.close();
                if(zis != null) is.close();
            } catch (IOException ex) {
                log.log(Level.WARNING, ex.getMessage(), ex);
            }
        }
        ReceiverThread.fileNumber++;
        System.out.print("Received file "+ReceiverThread.fileNumber+" - "+
                this.fileName+System.lineSeparator());
    }*/
    
}
