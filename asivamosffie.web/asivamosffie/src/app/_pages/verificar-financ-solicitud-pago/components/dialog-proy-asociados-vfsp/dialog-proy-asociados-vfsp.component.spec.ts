import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogProyAsociadosVfspComponent } from './dialog-proy-asociados-vfsp.component';

describe('DialogProyAsociadosVfspComponent', () => {
  let component: DialogProyAsociadosVfspComponent;
  let fixture: ComponentFixture<DialogProyAsociadosVfspComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogProyAsociadosVfspComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogProyAsociadosVfspComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
