import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';

@Component({
  selector: 'app-ver-detalleeditar-actuacion-reclm',
  templateUrl: './ver-detalleeditar-actuacion-reclm.component.html',
  styleUrls: ['./ver-detalleeditar-actuacion-reclm.component.scss']
})
export class VerDetalleeditarActuacionReclmComponent implements OnInit {
  
  controversiaId: any;
  reclamacionId: any;
  numReclamacion: any;
  idReclamacionActuacion:any;
  public codRecalamacion;
  public codReclamacionActuacion = localStorage.getItem('actuacionReclamacion');
  constructor(private activatedRoute: ActivatedRoute,private services: ContractualControversyService) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param => {
      this.controversiaId = param.idControversia;
      this.reclamacionId = param.idReclamacion;
      this.idReclamacionActuacion = param.id;
      this.services.GetControversiaActuacionById(this.reclamacionId).subscribe((data:any)=>{
        this.numReclamacion = data.numeroActuacionReclamacion;
      });
      
    });
  }

}
