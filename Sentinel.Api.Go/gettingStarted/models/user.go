package models

import (
	"errors"
	"fmt"
)

//User type
type User struct {
	ID        int
	FirstName string
	LastName  string
}

var (
	users  []*User
	nextID = 1
)

func GetUsers() []*User {
	return users
}

func AddUser(user User) (User, error) {
	var err error = nil
	if user.ID != 0 {
		return User{}, errors.New("New User's ID should be empty or 0")
	}
	//err = errors.New("Something went wrong")
	user.ID = nextID
	nextID++
	users = append(users, &user)
	return user, err

}

func GetUserByID(id int) (User, error) {
	for _, v := range users {
		if v.ID == id {
			return *v, nil
		}
	}
	return User{}, fmt.Errorf("User with ID '%v Not Found", id)
}

func UpdateUser(user User) error {
	for i, v := range users {
		if v.ID == user.ID {
			users[i] = &user
			return nil
		}
	}
	return fmt.Errorf("User with ID '%v Not Found", user.ID)
}

func RemoveUserByID(id int) error {
	for i, v := range users {
		if v.ID == id {
			users = append(users[:i], users[i+1:]...)
			return nil
		}
	}
	return fmt.Errorf("User with ID '%v Not Found", id)
}
