module com.lukaorocha.raycastrendering {
    requires javafx.controls;
    requires javafx.fxml;
    requires kotlin.stdlib;


    opens com.lukaorocha.raycastrendering to javafx.fxml;
    exports com.lukaorocha.raycastrendering;
}