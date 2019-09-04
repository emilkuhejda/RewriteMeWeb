import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateInformationMessageComponent } from './create-information-message.component';

describe('CreateInformationMessageComponent', () => {
  let component: CreateInformationMessageComponent;
  let fixture: ComponentFixture<CreateInformationMessageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateInformationMessageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateInformationMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
