import { User } from '../_models/user';

export class UserMapper {
    public static convertAll(data): User[] {
        let users = [];
        for (let item of data) {
            let user = UserMapper.convert(item);
            users.push(user);
        }

        return users;
    }

    public static convert(data): User {
        if (data === null || data === undefined)
            return null;

        let user = new User();
        user.id = data.id;
        user.email = data.email;
        user.givenName = data.givenName;
        user.familyName = data.familyName;
        user.dateRegistered = new Date(data.dateRegisteredUtc);

        return user;
    }
}
