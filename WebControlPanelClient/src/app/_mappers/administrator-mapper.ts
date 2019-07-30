import { Administrator } from '../_models/administrator';

export class AdministratorMapper {
    public static convertAll(data): Administrator[] {
        let administrators = [];
        for (let item of data) {
            let administrator = AdministratorMapper.convert(item);
            administrators.push(administrator);
        }

        return administrators;
    }

    public static convert(data): Administrator {
        let administrator = new Administrator();
        administrator.id = data.id;
        administrator.username = data.username;
        administrator.firstName = data.firstName;
        administrator.lastName = data.lastName;

        return administrator;
    }
}
