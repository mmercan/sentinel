package controllers

import (
	"encoding/json"
	"fmt"
	"io"
	"net/http"
	"regexp"
	"strconv"

	"mercan.io/pluralsight/webservice/models"
)

type userController struct {
	userIDPattern *regexp.Regexp
}

func (uc userController) ServeHTTP(w http.ResponseWriter, r *http.Request) {

	switch r.Method {
	case "GET":
		fmt.Println("Method GET")
	case "POST":
		fmt.Println("Method POST")
	case "PUT":
		fmt.Println("Method PUT")
	case "DELETE":
		fmt.Println("Method DELETE")
	default:
		fmt.Println(r.Method, " is not Defined")
	}

	w.Header().Add("Content-Type", "text/html")

	count := 5
	for i := 0; i <= count; i++ {
		if i == 2 {
			w.Write([]byte("Yes it is " + strconv.Itoa(i) + "<br/>"))
			continue
		} else if i == 5 {
			break
		}

		fmt.Println("Request received for userController")

		w.Write([]byte("Hello from userController " + strconv.Itoa(i) + "<br/>"))
	}

	slice := []int{1, 2, 3, 4, 5, 6}
	// for i := 0; i < len(slice); i++ {
	// 	fmt.Println(slice[i])
	// }
	for i, v := range slice {
		fmt.Println(i, v)
	}

	wellKnownPorts := map[string]int{"http": 80, "https": 443}

	for k, v := range wellKnownPorts {
		fmt.Println(k, v)
	}
}

func (uc *userController) getAll(w http.ResponseWriter, r *http.Request) {
	encodeResponseAsJson(models.GetUsers(), w)
}

func (uc *userController) get(id int, w http.ResponseWriter, r *http.Request) {
	u, err := models.GetUserByID(id)
	if err != nil {
		w.WriteHeader(http.StatusNotFound)
		return
	} else {
		encodeResponseAsJson(u, w)
	}
}

func (uc *userController) parseRequest(r *http.Request) (models.User, error) {
	dec := json.NewDecoder(r.Body)
	var u models.User
	err := dec.Decode(&u)
	if err != nil {
		return models.User{}, err
	}
	return u, nil
}

func newUserController() *userController {
	return &userController{
		userIDPattern: regexp.MustCompile(`^/users/(/d+)/?`),
	}
}

func encodeResponseAsJson(data interface{}, w io.Writer) {
	enc := json.NewEncoder(w)
	enc.Encode(data)
}
