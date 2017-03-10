/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package projekt5;

import DatabaseClasses.Department;
import DatabaseClasses.Student;
import java.net.URL;
import java.util.ResourceBundle;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.event.ActionEvent;
import javafx.event.EventHandler;
import javafx.fxml.FXML;
import javafx.fxml.Initializable;
import javafx.scene.control.ChoiceBox;
import javafx.scene.control.ContextMenu;
import javafx.scene.control.MenuItem;
import javafx.scene.control.TableColumn;
import javafx.scene.control.TableRow;
import javafx.scene.control.TableView;
import javafx.scene.control.TextField;
import javafx.scene.control.cell.ChoiceBoxTableCell;
import javafx.scene.control.cell.TextFieldTableCell;
import javafx.scene.input.MouseEvent;
import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.Persistence;

/**
 * FXML Controller class
 *
 * @author Zaki
 */
public class MainWindowController implements Initializable {

    @FXML
    private TableView<Student> studentCatalogTable;
    @FXML
    private TextField firstNameTextField;
    @FXML
    private TextField lastNameTextField;
    @FXML
    private TableView<Department> departmentCatalogTable;
    @FXML
    private ChoiceBox<Department> departmetnChoiceBox;
    @FXML
    private TableColumn<?, ?> studentDepartmentColumn;
    
    private ObservableList<Student> students;
    private ObservableList<Department> departments;
    private EntityManager entityManager;
    /**
     * Initializes the controller class.
     * @param url
     * @param rb
     */
    @Override
    public void initialize(URL url, ResourceBundle rb) {
        EntityManagerFactory emf = Persistence.createEntityManagerFactory("projekt5PU");
        entityManager = emf.createEntityManager();
        departments = FXCollections.observableArrayList(entityManager.createQuery("SELECT d FROM Department d").getResultList());
        departmentCatalogTable.setItems(departments);
        students = FXCollections.observableArrayList(entityManager.createQuery("SELECT s FROM Student s").getResultList());
        studentCatalogTable.setItems(students);
        
        departmetnChoiceBox.setConverter(new DepartmentStringConverter());
        departmetnChoiceBox.setItems(FXCollections.observableArrayList(departments));
        departmetnChoiceBox.getSelectionModel().select(0);
        
        setTableEditHandlers();
        
        studentCatalogTable.setRowFactory((TableView<Student> tableView) -> {
            final TableRow<Student> row = new TableRow<>();
            final ContextMenu rowMenu = new ContextMenu();
            MenuItem removeItem = new MenuItem("Delete");
            removeItem.setOnAction((ActionEvent event) -> {
                deleteItem(row.getItem());
                studentCatalogTable.getItems().remove(row.getItem());
            });
            rowMenu.getItems().addAll(removeItem);
            
            row.setContextMenu(rowMenu);
            return row;
        });
    }    

    @FXML
    private void handleFirstNameFieldClicked(MouseEvent event) {
        firstNameTextField.clear();
    }

    @FXML
    private void handleLastNameFieldClicked(MouseEvent event) {
        lastNameTextField.clear();
    }
    
    private void setTableEditHandlers(){
        studentCatalogTable.setEditable(true);
        departmentCatalogTable.setEditable(true);
        
        TableColumn column = studentCatalogTable.getColumns().get(0);
        column.setCellFactory(TextFieldTableCell.forTableColumn());
        column.setOnEditCommit(new EventHandler<TableColumn.CellEditEvent<Student, String>>() {
            @Override
            public void handle(TableColumn.CellEditEvent<Student, String> t) {
                Student s = t.getRowValue();
                String newName = t.getNewValue();
                s.setFirstname(newName);
                updateItem(s);
            }
        });
        
        column = studentCatalogTable.getColumns().get(1);
        column.setCellFactory(TextFieldTableCell.forTableColumn());
        column.setOnEditCommit(new EventHandler<TableColumn.CellEditEvent<Student, String>>() {
            @Override
            public void handle(TableColumn.CellEditEvent<Student, String> t) {
                Student s = t.getRowValue();
                String newName = t.getNewValue();
                s.setLastname(newName);
                updateItem(s);
            }
        });
        
        column = studentCatalogTable.getColumns().get(2);
        column.setCellFactory(ChoiceBoxTableCell.forTableColumn(new DepartmentStringConverter(), departments));
        column.setOnEditCommit(new EventHandler<TableColumn.CellEditEvent<Student, Department>>() {
            @Override
            public void handle(TableColumn.CellEditEvent<Student, Department> t) {
                Student s = t.getRowValue();
                Department newValue = t.getNewValue();
                s.setDepartmentid(newValue);
                updateItem(s);
            }
        });
        
        
        column = departmentCatalogTable.getColumns().get(0);
        column.setCellFactory(TextFieldTableCell.forTableColumn());
        column.setOnEditCommit(new EventHandler<TableColumn.CellEditEvent<Department, String>>() {
            @Override
            public void handle(TableColumn.CellEditEvent<Department, String> t) {
                Department d = t.getRowValue();
                String newName = t.getNewValue();
                d.setShortcut(newName);
                updateItem(d);
            }
        });
        
        column = departmentCatalogTable.getColumns().get(1);
        column.setCellFactory(TextFieldTableCell.forTableColumn());
        column.setOnEditCommit(new EventHandler<TableColumn.CellEditEvent<Department, String>>() {
            @Override
            public void handle(TableColumn.CellEditEvent<Department, String> t) {
                Department d = t.getRowValue();
                String newName = t.getNewValue();
                d.setFullname(newName);
                updateItem(d);
            }
        });
    }

    @FXML
    private void handleStudentCatalogMClicked(MouseEvent event) {
        if (!studentCatalogTable.getSelectionModel().isEmpty()){
            Student selectedItem = studentCatalogTable.getSelectionModel().getSelectedItem();
            Department itemsDepartment = selectedItem.getDepartmentid();
            departmentCatalogTable.getSelectionModel().select(itemsDepartment);
        }
    }
    
    private void updateItem(Object o){
        if (!studentCatalogTable.getSelectionModel().isEmpty()){
            Student selectedItem = studentCatalogTable.getSelectionModel().getSelectedItem();
            Department itemsDepartment = selectedItem.getDepartmentid();
            departmentCatalogTable.getSelectionModel().select(itemsDepartment);
        }
        try {
            entityManager.getTransaction().begin();
            entityManager.merge(o);
            entityManager.getTransaction().commit();
        } catch (Exception ex) {
            if (entityManager.getTransaction().isActive()) {
                entityManager.getTransaction().rollback();
            }
        }  
    }

    @FXML
    private void handleAddItem(ActionEvent event) {
        Student student = new Student();
        student.setFirstname(firstNameTextField.getText());
        student.setLastname(lastNameTextField.getText());
        student.setDepartmentid(departmetnChoiceBox.getSelectionModel().getSelectedItem());
        students.add(student);
        addItemToDatabase(student);
    }
    
    private void addItemToDatabase(Object o){
        try {
            entityManager.getTransaction().begin();
            entityManager.persist(o);
            entityManager.getTransaction().commit();
        } catch (Exception ex) {
            if (entityManager.getTransaction().isActive()) {
                entityManager.getTransaction().rollback();
            }
        } 
    }
    
    private void deleteItem(Object o){
        try {
            entityManager.getTransaction().begin();
            entityManager.remove(entityManager.merge(o));
            entityManager.getTransaction().commit();
        } catch (Exception ex) {
            if (entityManager.getTransaction().isActive()) {
                entityManager.getTransaction().rollback();
            }
        } 
    }

    @FXML
    private void handleReport(ActionEvent event) {
        Report r = new Report(entityManager);
    }
}
