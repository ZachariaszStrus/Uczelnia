/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package projekt2;

import java.io.File;
import java.io.IOException;
import java.net.URL;
import java.nio.file.Files;
import static java.nio.file.LinkOption.NOFOLLOW_LINKS;
import java.nio.file.Path;
import java.nio.file.attribute.DosFileAttributes;
import java.util.ResourceBundle;
import java.util.logging.Level;
import java.util.logging.Logger;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.fxml.Initializable;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.ContextMenu;
import javafx.scene.control.MenuItem;
import javafx.scene.control.MultipleSelectionModel;
import javafx.scene.control.TreeCell;
import javafx.scene.control.TreeItem;
import javafx.scene.control.TreeView;
import javafx.stage.DirectoryChooser;
import javafx.stage.Modality;
import javafx.stage.Stage;
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
    @FXML
    private MenuItem contextAdd;

    /**
     * Initializes the controller class.
     * @param url
     * @param rb
     */
    @Override
    public void initialize(URL url, ResourceBundle rb) {
        tree.setContextMenu(contextMenu);
        tree.setCellFactory(new FileCellFactory());
    }    

    @FXML
    private void chooseRoot(ActionEvent event) throws IOException {
        DirectoryChooser directoryChooser = new DirectoryChooser();
        File file = directoryChooser.showDialog(null);
        if(file.exists()){
            TreeItem<Path> root = new TreeItem<>();
            root.setExpanded(false);

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
    
    private void fillTree(Path file, TreeItem<Path> branch) throws IOException {
        TreeItem<Path> newBranch = makeBranch(file, branch);
        if(Files.isDirectory(file, NOFOLLOW_LINKS)){
            for (Path childFile : Files.newDirectoryStream(file)) {
                this.fillTree(childFile, newBranch);
            }
        }
    }

    @FXML
    private void deleteItem(ActionEvent event) throws IOException {
        if(!(tree.getSelectionModel().isEmpty())){
            TreeItem<Path> selectedItem = tree.getSelectionModel().getSelectedItem();
            this.deleteFiles(selectedItem);
            selectedItem.getParent().getChildren().remove(selectedItem);
        }
    }
    
    private void deleteFiles(TreeItem<Path> selectedItem) throws IOException{
        if(Files.isDirectory(selectedItem.getValue(), NOFOLLOW_LINKS)){
            for (TreeItem<Path> item : selectedItem.getChildren()) {
                this.deleteFiles(item);
            }
        }
        Files.delete(selectedItem.getValue());
    }
    

    @FXML
    private void addItem(ActionEvent event) throws IOException  {
        MultipleSelectionModel<TreeItem<Path>> selectionModel = tree.getSelectionModel();
        if(!(selectionModel.isEmpty()) && 
                Files.isDirectory(selectionModel.getSelectedItem().getValue(), NOFOLLOW_LINKS)){
            TreeItem<Path> selectedItem = selectionModel.getSelectedItem();
            FXMLLoader loader = new FXMLLoader(getClass().getResource("CreateDirectory.fxml"));
            Parent root = (Parent) loader.load();
            CreateDirectoryController controller = (CreateDirectoryController) loader.getController();
            Stage stage = new Stage();
            controller.setMyParameter(selectedItem, stage);
            stage.setScene(new Scene(root));
            stage.setTitle("Create directory");
            stage.initModality(Modality.WINDOW_MODAL);
            stage.initOwner(MainWindow.mainWindow); 
            stage.showAndWait();
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
    
    
}




