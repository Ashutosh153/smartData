import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewCompleteAppointmentsComponent } from './view-complete-appointments.component';

describe('ViewCompleteAppointmentsComponent', () => {
  let component: ViewCompleteAppointmentsComponent;
  let fixture: ComponentFixture<ViewCompleteAppointmentsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewCompleteAppointmentsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewCompleteAppointmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
