import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailContactFormComponent } from './detail-contact-form.component';

describe('DetailContactFormComponent', () => {
  let component: DetailContactFormComponent;
  let fixture: ComponentFixture<DetailContactFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetailContactFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailContactFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
