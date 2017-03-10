/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package projekt1;

import java.io.File;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.io.IOException;

/**
 *
 * @author Zaki
 */
public class Projekt1 {

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        try{
            DiskFile file = new DiskFile(Paths.get("F:\\zaki\\My files\\My files\\gra"));
            file.print();
        }
        catch(IOException e){
        }
            
    }
    
}
