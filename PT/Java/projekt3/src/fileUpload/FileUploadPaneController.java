/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package fileUpload;

import java.net.URL;
import java.util.ResourceBundle;
import javafx.fxml.FXML;
import javafx.fxml.Initializable;
import javafx.scene.control.Label;
import javafx.scene.control.ProgressBar;
import Client.SendWorker;

/**
 * FXML Controller class
 *
 * @author Zaki
 */
public class FileUploadPaneController implements Initializable {

    @FXML
    private ProgressBar progressBar;
    @FXML
    private Label messageLabel;
    @FXML
    private Label fileName;

    /**
     * Initializes the controller class.
     */
    @Override
    public void initialize(URL url, ResourceBundle rb) {
        // TODO
    }

    public void setProperties(SendWorker task){
        this.messageLabel.textProperty().bind(task.messageProperty());
        this.progressBar.progressProperty().bind(task.progressProperty());
        this.fileName.setText(task.getFileName());
    }
    
}
