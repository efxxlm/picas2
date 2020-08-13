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
    {name: 'Obra', value: '1'},
    {name: 'Interventoría', value: '2'}
  ];


  constructor(
                @Inject(MAT_DIALOG_DATA) public data: TableElement[],
                private projectContactingService: ProjectContractingService,
                public dialog: MatDialog,    
                public router: Router      
             ) 
  {
    
  }
  ngOnInit(): void {
    this.declararInput();

  }

  private declararInput() {
    this.solicitudAsociada = new FormControl('', [Validators.required]);
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  onSave(){

    let contratacion: Contratacion = {
      tipoSolicitudCodigo: this.solicitudAsociada.value,
      contratacionProyecto: []
    } 

    this.data.forEach( e => {
      
      let contratacionProyecto: ContratacionProyecto = {
        proyectoId: e.proyectoId,
      }

      contratacion.contratacionProyecto.push( contratacionProyecto )
    })

    this.projectContactingService.createContratacionProyecto( contratacion ).subscribe( respuesta => {
      this.openDialog( "Proceso seleccion", respuesta.message )
      if ( respuesta.code == "200" )
        this.router.navigate(["/solicitarContratacion/solicitud", respuesta.data.contratacionId]);
    })
  }

}
