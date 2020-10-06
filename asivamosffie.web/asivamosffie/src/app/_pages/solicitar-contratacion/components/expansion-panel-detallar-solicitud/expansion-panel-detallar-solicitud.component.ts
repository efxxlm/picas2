import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { ContratacionProyecto, Contratacion } from 'src/app/_interfaces/project-contracting';
import { ProjectContractingService } from 'src/app/core/_services/projectContracting/project-contracting.service';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-expansion-panel-detallar-solicitud',
  templateUrl: './expansion-panel-detallar-solicitud.component.html',
  styleUrls: ['./expansion-panel-detallar-solicitud.component.scss']
})
export class ExpansionPanelDetallarSolicitudComponent implements OnInit {

  contratacion: Contratacion = {};

  constructor(
              private route: ActivatedRoute,
              private projectContractingService: ProjectContractingService,
              public dialog: MatDialog,    
              private router: Router,

  ) 
  { }

  ngOnInit(): void {
    this.route.params.subscribe((params: Params) => {
      this.contratacion.contratacionId = params.id;

      this.projectContractingService.getContratacionByContratacionId( this.contratacion.contratacionId )
        .subscribe( response => {
            this.contratacion = response;

            setTimeout(() => {

              let btnTablaProyectos = document.getElementById('btnTablaProyectos');
              let btnTablaContratistas = document.getElementById('btnTablaContratistas');
              let btnTablacaracteristicas = document.getElementById('btnTablacaracteristicas');
              let btnconsideraciones = document.getElementById('btnconsideraciones');
              let btnFuentesUsos = document.getElementById('btnFuentesUsos');

              btnTablaProyectos.click();
              btnTablaContratistas.click();
              btnTablacaracteristicas.click();
              btnconsideraciones.click();
              btnFuentesUsos.click();

            }, 1000);

            console.log( response );
      })

    });
  }

  openDialog(modalTitle: string, modalText: string) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });   
  }

  onSubmit(){
    console.log( this.contratacion );

    this.projectContractingService.createEditContratacion( this.contratacion )
      .subscribe( respuesta => {
        this.openDialog( "Solicitud Contratación", respuesta.message )

        console.log( respuesta );

        if (respuesta.code == "200")
          this.router.navigate(["/solicitarContratacion/solicitud", this.contratacion.contratacionId ]);

      })

  }

}
