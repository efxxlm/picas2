import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AsociadaComponent } from './asociada.component';

describe('AsociadaComponent', () => {
  let component: AsociadaComponent;
  let fixture: ComponentFixture<AsociadaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AsociadaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AsociadaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
