/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package Client;

import server.ServerThread;
import fileUpload.UploadStatusWindow;
import java.io.File;
import java.io.IOException;
import java.net.URL;
import java.nio.file.Files;
import static java.nio.file.LinkOption.NOFOLLOW_LINKS;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.nio.file.attribute.DosFileAttributes;
import java.util.ResourceBundle;
import java.util.logging.Level;
import java.util.logging.Logger;
import javafx.beans.property.ObjectProperty;
import javafx.beans.property.SimpleObjectProperty;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.Initializable;
import javafx.scene.control.CheckBox;
import javafx.scene.control.ContextMenu;
import javafx.scene.control.SelectionMode;
import javafx.scene.control.TextField;
import javafx.scene.control.TreeCell;
import javafx.scene.control.TreeItem;
import javafx.scene.control.TreeView;
import javafx.stage.DirectoryChooser;
import javafx.util.Callback;

/**
 * FXML Controller class
 *
 * @author Zaki
 */
public class MainController implements Initializable {

    @FXML
    private TreeView<Path> tree;
    @FXML
    private ContextMenu contextMenu;

    static ObjectProperty<ServerThread> server = new SimpleObjectProperty<>();
    @FXML
    private TextField serverNameTextField;
    @FXML
    private TextField portNumberTextField;
    @FXML
    private CheckBox compressBox;
    @FXML
    private CheckBox encryptBox;
    /**
     * Initializes the controller class.
     * @param url
     * @param rb
     */
    @Override
    public void initialize(URL url, ResourceBundle rb) {
        this.setDefaultRoot(Paths.get("F:\\Downloads"));
        tree.setCellFactory(new FileCellFactory());
        tree.getSelectionModel().setSelectionMode(SelectionMode.MULTIPLE);
        this.startServerAction();
    }    

    private void setDefaultRoot(Path r){
            TreeItem<Path> root = new TreeItem<>();
            root.setExpanded(true);

            this.fillTree(r , root);
            tree.setRoot(root);
            tree.setShowRoot(false);
    }
    
    @FXML
    private void chooseRoot(ActionEvent event){
        DirectoryChooser directoryChooser = new DirectoryChooser();
        File file = directoryChooser.showDialog(null);
        if(file.exists()){
            TreeItem<Path> root = new TreeItem<>();
            root.setExpanded(true);

            this.fillTree(file.toPath() , root);
            tree.setRoot(root);
            tree.setShowRoot(false);
        }
        
    }

    public static TreeItem<Path> makeBranch(Path file, TreeItem<Path> parent) {
        TreeItem<Path> item = new TreeItem<>(file);
        item.setExpanded(false);
        parent.getChildren().add(item);
        return item;
    }
    
    private void fillTree(Path file, TreeItem<Path> branch) {
        TreeItem<Path> newBranch = makeBranch(file, branch);
        if(Files.isDirectory(file, NOFOLLOW_LINKS)){
            try {
                for (Path childFile : Files.newDirectoryStream(file)) {
                    this.fillTree(childFile, newBranch);
                }
            } catch (IOException ex) {
                System.out.print("Exception in 'fillTree' func : " + ex);
            }
        }
    }

    @FXML
    private void handleUploadButton(ActionEvent event) throws IOException {
        ObservableList<TreeItem<Path>> items = this.tree.getSelectionModel().getSelectedItems();
        
        try {
            for(TreeItem<Path> item : items){
                Path path = item.getValue();
                
                SendWorker task = new SendWorker(path, this.serverNameTextField.getText(),
                        Integer.parseInt(this.portNumberTextField.getText()),
                        this.compressBox.isSelected(), this.encryptBox.isSelected());
                
                UploadStatusWindow uploadWindow = new UploadStatusWindow();
                uploadWindow.update(task);
                
                Thread th = new Thread(task);
                th.start();
            }
            
        } catch (NumberFormatException ex) {
            System.out.print("Exception in 'handleUploadButton' func : " + ex + System.lineSeparator());
        }
    }
    
    @FXML
    private void handleTreeAction(javafx.scene.input.MouseEvent event) {
        if(!this.tree.getSelectionModel().isEmpty()){
            for (TreeItem<Path> item : this.tree.getSelectionModel().getSelectedItems()) {
                if(Files.isDirectory(item.getValue())){
                    tree.setContextMenu(null);
                    return;
                }
            }
            tree.setContextMenu(contextMenu);
        }
        else{
            tree.setContextMenu(null);
        }
    }

    private class FileCell extends TreeCell<Path> {
    
        @Override
        protected void updateItem(Path file, boolean empty) {
            super.updateItem(file, empty);
            if (file != null) {
                try {
                    DosFileAttributes attr =
                        Files.readAttributes(file, DosFileAttributes.class);
                    String name = file.getFileName()+" ";
                    name += attr.isReadOnly() ? "r" : "-";
                    name += attr.isArchive() ? "a" : "-";
                    name += attr.isHidden() ? "h" : "-";
                    name += attr.isSystem() ? "s" : "-";
                    setText(name);
                } catch (IOException ex) {
                    Logger.getLogger(MainController.class.getName()).log(Level.SEVERE, null, ex);
                }
            } else {
                setText(null);
            }
        }
    }
    
    private class FileCellFactory implements Callback<TreeView<Path>, TreeCell<Path>> {
        @Override
        public TreeCell<Path> call(TreeView<Path> p) {
            return new FileCell();
        }
    }
    
    private void startServerAction() {
        server.set(new ServerThread());
        Thread th = new Thread(server.get());
        th.start();
    }

    public static void stopServerAction() {
        server.get().stop();
        server.set(null);
    }
}




