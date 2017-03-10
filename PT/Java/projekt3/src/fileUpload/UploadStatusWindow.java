/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package fileUpload;

import java.io.IOException;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.control.ScrollBar;
import javafx.scene.layout.Pane;
import javafx.scene.layout.VBox;
import javafx.stage.Stage;
import Client.SendWorker;

/**
 *
 * @author Zaki
 */
public class UploadStatusWindow {
    private Pane mainPane = null;
    private Scene scene = null;
    private Stage uploadWindow = null;
    private ScrollBar scrollBar = new ScrollBar();
    private static int nextPosX = 50;
    private static int nextPosY = 50;
    
    public UploadStatusWindow() {
        mainPane = new VBox();
    }
    
    public void update(SendWorker task) {
        try {
            //add pane
            FXMLLoader fxmlLoader = new FXMLLoader();
            Pane uploadFilePane = fxmlLoader.load(getClass().getResource("FileUploadPane.fxml").openStream());
            
            mainPane.getChildren().add(uploadFilePane);
            
            //set progress bar and status label
            FileUploadPaneController controller = 
                    (FileUploadPaneController) fxmlLoader.getController();
            controller.setProperties(task);
            
        } catch (IOException ex) {
            System.out.print("Exception in 'showUploadWindow' func : " + ex + System.lineSeparator());
        }
        
        if(this.uploadWindow == null){
            this.show(task);
        }
    }
    
    public void show(SendWorker task){
        //add scene
        scene = new Scene(mainPane);

        //stage
        uploadWindow = new Stage();
        uploadWindow.setX(nextPosX);
        uploadWindow.setY(nextPosY);
        uploadWindow.setTitle("Upload status");
        uploadWindow.setMaxHeight(500);
        uploadWindow.setScene(scene);
        uploadWindow.show();
        
        uploadWindow.setOnCloseRequest(e -> {
            task.cancel();
        });
        
        setNewPosition();
    }
    
    private void setNewPosition(){
        if(nextPosX > 500 && nextPosY > 500){
            nextPosX = 50;
            nextPosY = 50;
        } else {
            nextPosX += 20;
            nextPosY += 20;
        }
    }
}
