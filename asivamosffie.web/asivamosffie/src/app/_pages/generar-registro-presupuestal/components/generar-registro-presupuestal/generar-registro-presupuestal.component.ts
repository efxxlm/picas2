import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DisponibilidadPresupuestalService } from 'src/app/core/_services/disponibilidadPresupuestal/disponibilidad-presupuestal.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-generar-registro-presupuestal',
  templateUrl: './generar-registro-presupuestal.component.html',
  styleUrls: ['./generar-registro-presupuestal.component.scss']
})
export class GenerarRegistroPresupuestalComponent implements OnInit {
  verAyuda = false;
  lista: any[];
  constructor() { }

  ngOnInit(): void {
    
  }


}
