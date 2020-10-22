import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { ProjectService } from 'src/app/core/_services/project/project.service';
import { Contratacion, ContratacionProyecto } from 'src/app/_interfaces/project-contracting';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { Router } from '@angular/router';

export interface TableElement {
  id: number;
  proyectoId: number;
  tipoInterventor: string;
  llaveMEN: string;
  region: string;
  departamento: string;
  municipio: string;
  institucionEducativa: string;
  sede: string;
}

@Component({
  selector: 'app-asociada',
  templateUrl: './asociada.component.html',
  styleUrls: ['./asociada.component.scss']
})
export class AsociadaComponent implements OnInit {

  solicitudAsociada: FormControl;
  solicitudAsociadaArray = [
    { name: 'Obra', value: '1' },
    { name: 'Interventoría', value: '2' }
  ];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private projectContactingService: ProjectContractingService,
    private dialogRef: MatDialogRef<AsociadaComponent>,
    public dialog: MatDialog,
    public router: Router
  ) {

  }
  ngOnInit(): void {
    this.declararInput();
  }

  private declararInput() {
    this.solicitudAsociada = new FormControl('', [Validators.required]);
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  validaciones(): string {
    if (this.data.esMultiproyecto === 'true' && this.data.data.length < 2) {
      return '<b>Debe seleccionar por lo menos dos (2) proyectos</b>';
    }

    if (this.data.esMultiproyecto === 'false' && this.data.data.length > 1) {
      return '<b>Puede seleccionar unicamente un (1) proyecto</b>';
    }
    return '';
  }

  onSave() {

    const contratacion: Contratacion = {
      tipoSolicitudCodigo: this.solicitudAsociada.value.length > 1 ? '3' : this.solicitudAsociada.value[0],
      contratacionProyecto: []
    };

    const mensajeValidaciones = this.validaciones();

    if (mensajeValidaciones.length > 0) {
      this.openDialog('', mensajeValidaciones)
      return false;
    };

    this.data.data.forEach(e => {

      const contratacionProyecto: ContratacionProyecto = {
        proyectoId: e.proyectoId,
      }

      contratacion.contratacionProyecto.push(contratacionProyecto)
    });

    this.projectContactingService.createContratacionProyecto(contratacion).subscribe(
      respuesta => {
        this.dialogRef.close();
        if (respuesta.code === '200') {
          this.openDialog('', respuesta.message);
        }
        this.router.navigate(['/solicitarContratacion']);
      },
      err => this.openDialog('', err.message)
    );

  }

}
