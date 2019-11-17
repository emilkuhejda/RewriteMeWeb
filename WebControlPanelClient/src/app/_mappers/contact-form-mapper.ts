import { ContactForm } from '../_models/contact-form';

export class ContactFormMapper {
    public static convertAll(data): ContactForm[] {
        let contactForms = [];
        for (let item of data) {
            let contactForm = ContactFormMapper.convert(item);
            contactForms.push(contactForm);
        }

        return contactForms;
    }

    public static convert(data): ContactForm {
        if (data === null || data === undefined)
            return null;

        let contactForm = new ContactForm();
        contactForm.id = data.id;
        contactForm.name = data.name;
        contactForm.email = data.email;
        contactForm.message = data.message;
        contactForm.dateCreated = new Date(data.dateCreatedUtc);

        return contactForm;
    }
}
