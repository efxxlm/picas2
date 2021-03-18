import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-observaciones',
  templateUrl: './dialog-observaciones.component.html',
  styleUrls: ['./dialog-observaciones.component.scss']
})
export class DialogObservacionesComponent implements OnInit {

  contratacionProyecto: any;
  observacion: string;

  constructor (
    @Inject(MAT_DIALOG_DATA) public data,
    private projectContractingSvc: ProjectContractingService )
  {

  }

  ngOnInit(): void {
    this.projectContractingSvc.getListContratacionObservacion( this.data.contratacionId )
      .subscribe(
        response => {
          this.contratacionProyecto = response[ 'contratacionObservacion' ];
          this.observacion = response[ 'observacionNotMapped' ];
        }
      );
  }

  innerObservacion ( observacion: string ) {
    const observacionHtml = observacion.replace( '"', '' );
    return observacionHtml;
  };

}
