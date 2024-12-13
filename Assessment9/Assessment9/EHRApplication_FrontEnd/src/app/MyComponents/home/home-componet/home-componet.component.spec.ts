import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeComponetComponent } from './home-componet.component';

describe('HomeComponetComponent', () => {
  let component: HomeComponetComponent;
  let fixture: ComponentFixture<HomeComponetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HomeComponetComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HomeComponetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
