import React, { ChangeEvent, useEffect, useState } from "react";
import { FormEvent } from "react";
import axios from "axios";

export default function enter({}) {
  const [firstName, setFirstName] = useState<string>("");
  const [lastName, setLastName] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");

  const getUser = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const response = await axios.get("http://localhost:5209/api/Users");
    console.log(response);
  };

  const addUser = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();

    type User = {
      name: string;
      lastName: string;
      email: string;
      password: string;
    };

    try {
      await axios.post<User>(
        "http://localhost:5209/api/Users",
        {
          Name: firstName,
          LastName: lastName,
          Email: email,
          Password: password,
        },
        {
          headers: {
            "Content-Type": "application/json",
            Accept: "application/json",
          },
        }
      );
    } catch (error) {
      if (axios.isAxiosError(error)) {
        console.log("error message: ", error.message);
        // 👇️ error: AxiosError<any, any>
        return error.message;
      } else {
        console.log("unexpected error: ", error);
        return "An unexpected error occurred";
      }
    }
  };

  return (
    <>
      <main>
        Sign Up
        <form onSubmit={addUser}>
          <label>
            FirstName:
            <input
              type="text"
              value={firstName}
              onChange={(e: ChangeEvent<HTMLInputElement>) =>
                setFirstName(e.target.value)
              }
            />
          </label>
          <label>
            LastName:
            <input
              type="text"
              value={lastName}
              onChange={(e) => setLastName(e.target.value)}
            />
            <label>
              Email:
              <input
                type="text"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </label>
            <label>
              Password:
              <input
                type="text"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </label>
          </label>
          <button type="submit">Submit</button>
        </form>
        <form onSubmit={getUser}>
          <button>Get</button>
        </form>
      </main>
    </>
  );
}
