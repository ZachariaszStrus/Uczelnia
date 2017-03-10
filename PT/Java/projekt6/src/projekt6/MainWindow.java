/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package projekt6;

import java.io.IOException;
import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.layout.Pane;
import javafx.stage.Stage;

/**
 *
 * @author Zaki
 */
public class MainWindow extends Application {
    
    @Override
    public void start(Stage primaryStage) throws IOException {
        Pane mainPane = FXMLLoader.load(getClass().getResource("MainWindow.fxml"));
        Scene scene = new Scene(mainPane);
        
        primaryStage.setTitle("Application 6");
        primaryStage.setScene(scene);
        primaryStage.show();
    }

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        launch(args);
    }
    
}
