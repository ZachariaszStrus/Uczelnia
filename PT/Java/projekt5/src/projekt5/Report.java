/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package projekt5;

import DatabaseClasses.Department;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;
import javafx.collections.ObservableList;
import javafx.geometry.Insets;
import javafx.scene.Scene;
import javafx.scene.control.Label;
import javafx.scene.layout.StackPane;
import javafx.scene.layout.VBox;
import javafx.stage.Stage;
import javax.persistence.EntityManager;

/**
 *
 * @author Zaki
 */
public class Report {
    public Report(EntityManager entityManager){
        VBox pane = new VBox();
        pane.setPadding(new Insets(20, 20, 20, 20));
        pane.setSpacing(10.0);
        List<Object[]> departments = entityManager.createQuery(
                            "SELECT s.departmentid, Count(s)"
                                    + "FROM Student s "
                                    + "GROUP BY s.departmentid"
                        ).getResultList();
        departments.sort((Object[] t, Object[] t1) -> ((Number)t[1]).intValue() - ((Number)t1[1]).intValue());
        
        for(Object[] d : departments){
            pane.getChildren().add(new Label(((Department)d[0]).getFullname()+" : \t"+d[1]));
        }
        
        Scene scene = new Scene(pane);
        Stage report = new Stage();
        report.setTitle("Report");
        report.setScene(scene);
        report.show();
    }
    
    
}
