import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PractionerRegestrationComponent } from './practioner-regestration.component';

describe('PractionerRegestrationComponent', () => {
  let component: PractionerRegestrationComponent;
  let fixture: ComponentFixture<PractionerRegestrationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PractionerRegestrationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PractionerRegestrationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
