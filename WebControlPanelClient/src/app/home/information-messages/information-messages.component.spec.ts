import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InformationMessagesComponent } from './information-messages.component';

describe('InformationMessagesComponent', () => {
  let component: InformationMessagesComponent;
  let fixture: ComponentFixture<InformationMessagesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InformationMessagesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InformationMessagesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
