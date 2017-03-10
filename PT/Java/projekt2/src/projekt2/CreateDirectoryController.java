/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package projekt2;

import java.io.IOException;
import java.net.URL;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ResourceBundle;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.Initializable;
import javafx.scene.control.Button;
import javafx.scene.control.CheckBox;
import javafx.scene.control.TextField;
import javafx.scene.control.TreeItem;
import javafx.stage.Stage;

/**
 * FXML Controller class
 *
 * @author Zaki
 */
public class CreateDirectoryController implements Initializable {

    private TreeItem<Path> selectedItem;
    private Stage stage;
    private boolean file;
    
    @FXML
    private CheckBox checkBox1;
    @FXML
    private CheckBox checkBox2;
    @FXML
    private Button submitButton;
    @FXML
    private TextField fileName;
    /**
     * Initializes the controller class.
     */
    @Override
    public void initialize(URL url, ResourceBundle rb) {
        // TODO
    }    

    void setMyParameter(TreeItem<Path> item, Stage s) {
        this.selectedItem = item;
        this.stage = s;
    }

    @FXML
    private void checkBox1Handler(ActionEvent event) {
        if(checkBox2.isSelected())
            checkBox2.setSelected(false);
        
        checkBox1.setSelected(true);
        this.file = true;
        this.submitButton.setDisable(false);
    }

    @FXML
    private void checkBox2Handler(ActionEvent event) {
        if(checkBox1.isSelected())
            checkBox1.setSelected(false);
        
        checkBox2.setSelected(true);
        this.file = false;
        this.submitButton.setDisable(false);
    }

    @FXML
    private void createFile(ActionEvent event) throws IOException {
        if(!this.fileName.getText().isEmpty()){
            String newDir = this.selectedItem.getValue().toString()+"\\"+this.fileName.getText();
            Path path = Paths.get(newDir);
            if(this.file){
                Files.createFile(path);
            }
            else{
                Files.createDirectory(path);
            }
            MainController.makeBranch(path, this.selectedItem);

            this.stage.close();
        }
    }
    
}
