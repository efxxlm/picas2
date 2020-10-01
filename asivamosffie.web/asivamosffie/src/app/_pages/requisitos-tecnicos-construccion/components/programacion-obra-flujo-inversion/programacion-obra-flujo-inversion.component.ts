import { Component, Input, OnInit } from '@angular/core';
import { DialogCargarProgramacionComponent } from '../dialog-cargar-programacion/dialog-cargar-programacion.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-programacion-obra-flujo-inversion',
  templateUrl: './programacion-obra-flujo-inversion.component.html',
  styleUrls: ['./programacion-obra-flujo-inversion.component.scss']
})
export class ProgramacionObraFlujoInversionComponent implements OnInit {

  @Input() esFlujoInversion: boolean;

  constructor ( private dialog: MatDialog ) { }

  ngOnInit(): void {
  }

  cargarProgramacion () {
    const dialogCargarProgramacion = this.dialog.open( DialogCargarProgramacionComponent, {
      width: '75em'
    });

    dialogCargarProgramacion.afterClosed().subscribe( resp => {
      console.log( resp );
    } );
  };

}
