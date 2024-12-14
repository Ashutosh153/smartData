import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdatePractitionerProfileComponent } from './update-practitioner-profile.component';

describe('UpdatePractitionerProfileComponent', () => {
  let component: UpdatePractitionerProfileComponent;
  let fixture: ComponentFixture<UpdatePractitionerProfileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UpdatePractitionerProfileComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UpdatePractitionerProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
