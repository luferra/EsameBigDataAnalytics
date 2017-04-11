// Created by StreamLoader on 02/11/2016 14:00:58;
package spark_translate;
import org.apache.spark.sql.SparkSession;
import org.apache.spark.sql.Column;
import org.apache.spark.sql.Dataset;
import org.apache.spark.sql.Row;
import static org.apache.spark.sql.functions.col;


public class spark_script {
	public static void main(String[] args) throws Exception {
	SparkSession spark = SparkSession
	.builder()
	.appName("Java Spark Translator")
	.master("local")
	.getOrCreate();

	Dataset<Row> s1 = spark.read().json("rainfall.json");

	Dataset<Row> f1 = s1.filter((col("rainfall").equalTo("25")).and(col("city_name").contains("pip")));


// Created by StreamLoader on 02/11/2016 14:00:58;
package spark_translate;
import org.apache.spark.sql.SparkSession;
import org.apache.spark.sql.Column;
import org.apache.spark.sql.Dataset;
import org.apache.spark.sql.Row;
import static org.apache.spark.sql.functions.col;


public class spark_script {
	public static void main(String[] args) throws Exception {
	SparkSession spark = SparkSession
	.builder()
	.appName("Java Spark Translator")
	.master("local")
	.getOrCreate();

	Dataset<Row> s1 = spark.read().json("rainfall.json");

	Dataset<Row> f1 = s1.filter((col("rainfall").equalTo("25")).and(col("city_name").contains("pip")));


