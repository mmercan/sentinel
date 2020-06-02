package main

import (
	"fmt"
	"net/http"
	"strconv"

	"mercan.io/pluralsight/webservice/controllers"
	"mercan.io/pluralsight/webservice/models"
)

const (
	pi   float32 = 3.14 //Create and assing together simplfy with => f:=3.14
	i            = 42
	aa           = 2 << iota // iota give you the order of the const group startig from 0 (this is the third item value will be 2) you can have multiple const group and every group starts from 0
	aaa          = 2 << iota
	aaaa         = iota + 6
)

func useStruct() {
	//u := models.User
	var u models.User
	u.ID = 67
	u.FirstName = "Bkla"
	u.LastName = "kkhhui"

	fmt.Println(u)

	u2 := models.User{ID: 3, FirstName: "Matt", LastName: "Mercan"}

	fmt.Println(u2)
}

func arrayItem() [3]int {
	var arr [3]int //define an array
	arr[0] = 19    //asign 19 to the first item of the array
	fmt.Println(arr)
	fmt.Println(arr[1])
	return arr //return it
}

func slicePlay() {
	slice := []int{1, 2, 3}
	slice = append(slice, 4, 47, 105, 66)
	fmt.Println(slice)

	s1 := slice[:1]
	s2 := slice[2:]
	s3 := slice[1:3]

	fmt.Println("s1", s1)
	fmt.Println("s2", s2)
	fmt.Println("s3", s3)
	//	arr := [3]int{1, 2, 3} //define an array
	//	slice := arr[:]        //create slice from the array
	//	arr[2] = 5
	//	slice[1] = 10
	//	fmt.Println(arr, slice) //slice points to array and items in both array and slice will be same
}

func mapPlay() {
	mm := map[string]int{}
	mm["matt"] = 890
	mm["test"] = 990
	mm["bey"] = 1990
	fmt.Println(mm)

	delete(mm, "test")
	fmt.Println(mm)
}

func complexNumbers(realnum float32, imaginary float32) {
	c := complex(realnum, imaginary) //create a complex number
	fmt.Println(c)                   // print the complex number
	r, im := real(c), imag(c)        //get back real and imaginary part of the complex number
	fmt.Println(r, im)               // print them out
}

func mymain() {
	fmt.Println(aa, aaa, aaaa)

	var st = strconv.Itoa(i)               //Convert int to string with Itoa
	fmt.Println("Lucky Number is "+st, pi) //print them all
	fmt.Println(float32(i) + 1.2)          //convert in to float before sum
	firstname := "Matt"                    // Short Hand Variable Create
	var lastname *string = new(string)     // Pointer Create
	lastname = &firstname                  // Assing Address of the variable to the Pointer
	fmt.Println(lastname, *lastname)       //Print Pointer (it will print location address)
	firstname = "blah"                     //change the variable value
	fmt.Println(lastname, *lastname)       // when the variable changes location still the same but value is changed

	fmt.Println("Hello from Module, " + *lastname + " " + firstname) //print *lastname => content of the pointer points

	complexNumbers(4, 3)

	arr := arrayItem()

	fmt.Println(arr) //print array

	slicePlay()
	mapPlay()
	useStruct()
}

func main() {
	usersadded()
	port := 3000
	_, err := startWebServer(port)
	if err != nil {
		fmt.Println("is server start failed", err)
	} else {
		fmt.Println("is server started")
	}

	// mymain()
}

func startWebServer(port int) (int, error) {
	var err error = nil

	fmt.Println("Server Starting...")
	//do important things

	controllers.RegisterControllers()
	http.ListenAndServe(":3000", nil)

	fmt.Println("Server Started on port", port)
	//err = errors.New("Something went wrong")
	return port, err
}

func usersadded() {
	u1 := models.User{ID: 1, FirstName: "Matt", LastName: "Mercan"}
	u2 := models.User{ID: 2, FirstName: "Esra", LastName: "Mercan"}

	if u1.ID == u2.ID && u1.FirstName == u2.FirstName {
		fmt.Println("Same user")
	} else if u1.LastName == u2.LastName {
		fmt.Println("Same Family")
	} else {
		fmt.Println("Not the same user")
	}
}
