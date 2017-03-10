/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package projekt1;

import java.io.File;
import java.io.IOException;
import java.nio.file.Files;
import static java.nio.file.LinkOption.NOFOLLOW_LINKS;
import java.nio.file.Path;
import java.text.SimpleDateFormat;
import java.util.Comparator;
import java.util.Date;
import java.util.HashSet;
import java.util.Set;
import java.util.TreeSet;

enum fileKind{
    REGULAR_FILE,
    DIRECTORY;
}

/**
 *
 * @author Zaki
 */
public class DiskFile implements Comparable{
    
    public DiskFile(Path dir) throws IOException{
        this.name = dir.getFileName().toString();
        this.kind = (Files.isDirectory(dir, NOFOLLOW_LINKS) ? fileKind.DIRECTORY : fileKind.REGULAR_FILE);
        this.lastModified = Files.getLastModifiedTime(dir, NOFOLLOW_LINKS).toString();
        this.size = Files.size(dir);
        this.childFiles = new TreeSet();
        if(Files.isDirectory(dir, NOFOLLOW_LINKS)){
            for (Path file : Files.newDirectoryStream(dir)) {
                this.childFiles.add(new DiskFile(file));
            }
        }
    }
    
    public DiskFile(Path dir, boolean tmp) throws IOException{
        this.name = dir.getFileName().toString();
        this.kind = (Files.isDirectory(dir, NOFOLLOW_LINKS) ? fileKind.DIRECTORY : fileKind.REGULAR_FILE);
        this.lastModified = Files.getLastModifiedTime(dir, NOFOLLOW_LINKS).toString();
        this.size = Files.size(dir);
        this.childFiles = new TreeSet();
    }
    
    public void print(){
        this.print(this, 1);
    }
    
    private void print(DiskFile diskFile, int n){
        for (int i = 0; i < n; i++) {
            System.out.print("   ");
        }
        System.out.print(diskFile.toString()+System.lineSeparator());
        if(this.childFiles.size() > 0){
            for (DiskFile dFile : diskFile.childFiles) {
                dFile.print(dFile, n+1);
            }
        }
    }
    
    private long size(){
        if(this.kind == fileKind.REGULAR_FILE){
            return this.size;
        }
        else{
            return this.childFiles.size();
        }
    }
    
    @Override
    public String toString(){
        return this.name+" "+
                "["+this.lastModified+"]"+" "+
                (this.kind == fileKind.DIRECTORY ? "K" : "P")+" "+
                "("+this.size()+")";
    }

    @Override
    public int compareTo(Object t) {
        DiskFile tmp = (DiskFile)t;
        if(this.kind != tmp.kind){
            return this.kind == fileKind.DIRECTORY ? 1 : -1;
        }
        else{
            return this.size() > tmp.size() ? 1 : -1;
        }
    }
    
    public String getName(){
        return this.name;
    }
    public long getSize(){
        return this.size;
    }
    public fileKind getKind(){
        return this.kind;
    }
    public String getLastModifiedTime(){
        return this.lastModified;
    }
    
    private Set<DiskFile> childFiles;
    private String name;
    private long size;
    private fileKind kind;
    private String lastModified;
}
